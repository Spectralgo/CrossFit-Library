<template>
  <div class="text-center">

    <v-stepper v-model="step">
      <v-stepper-header>
        <v-stepper-step :complete="step > 1" step="1">Trick Information</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step step="2">Review</v-stepper-step>
      </v-stepper-header>

      <v-stepper-items>

        <v-stepper-content step="1">
          <div>
            <v-text-field v-model="form.name" label="Tricking Name"></v-text-field>
            <v-text-field v-model="form.description" label="Description"></v-text-field>

            <v-select :items="difficultyItems" label="Difficulty"
                      @change="selectDifficulty">
            </v-select>

            <v-select :items="categoryItems" label="Categories"
                      @change="selectCategories" multiple chips small-chips deletable-chips>
            </v-select>

            <v-select :items="trickItems" label="Prerequisites"
                      @change="selectPrerequisites" multiple chips small-chips deletable-chips >
            </v-select>

            <v-select :items="trickItems" label="Progressions"
                      @change="selectProgressions" multiple chips small-chips deletable-chips >
            </v-select>

            <v-btn @click="step++">Next</v-btn>
          </div>
        </v-stepper-content>

        <v-stepper-content step="2">
          <div>
            <v-btn @click="save">Save</v-btn>
          </div>
        </v-stepper-content>
      </v-stepper-items>
    </v-stepper>
  </div>
</template>

<script>
import {mapActions, mapGetters, mapMutations, mapState} from 'vuex';

const initSate = () => ({
  form: {
    name: "",
    description: "",
    difficulty: "",
    prerequisites: [],
    progressions: [],
    categories: [],
  },
  submission: "",
  step: 1,
})

export default {
  name: "trick-steps",
  data: initSate,
  computed: {
    ...mapGetters('tricks', ['trickItems', 'categoryItems', 'difficultyItems']),
  },
  methods: {
    ...mapMutations('video-upload', ['reset']),
    ...mapActions('tricks', ['createTrick']),
    async save() {
      await this.createTrick({
        form: this.form
      });
      this.reset();
      Object.assign(this.$data, initSate())
    },
    selectDifficulty(item) {
      return this.form.difficulty = item;
    },
    selectCategories(item) {
      return this.form.categories = item;
    },
    selectPrerequisites(item) {
      return this.form.prerequisites = item;
    },
    selectProgressions(item) {
      return this.form.progressions = item;
    },
  }
}


</script>

<style scoped>

</style>

