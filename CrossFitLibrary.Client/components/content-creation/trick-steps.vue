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
            <v-text-field v-model="form.trickName" label="Tricking Name"></v-text-field>
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
import {mapActions, mapMutations} from 'vuex';

const initSate = () => ({
    form: {
      trickName: "",
    },
    submission: "",
    step: 1
  })

export default {
  name: "trick-steps",
  data: initSate,
  methods: {
    ...mapMutations('video-upload', ['reset']),
    ...mapActions('tricks', ['createTrick']),
    async save() {
      await this.createTrick({
        form: this.form
      });
      this.reset();
      Object.assign(this.$data, initSate())
    }
  }
}


</script>

<style scoped>

</style>

