﻿<template>
  <div class="text-center">
    <v-card>
      <v-card-title>
        Create Trick
        <v-spacer></v-spacer>
        <v-btn icon @click="close">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-card-title>
      <v-stepper v-model="step" rounded="0">
        <v-stepper-header class="elevation-0">
          <v-stepper-step :complete="step > 1" step="1">Trick Information</v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step step="2">Review</v-stepper-step>
        </v-stepper-header>

        <v-stepper-items class="fmb-1-for-btn">

          <v-stepper-content step="1">
            <div>
              <v-text-field v-model="form.name" label="Tricking Name"></v-text-field>
              <v-text-field v-model="form.description" label="Description"></v-text-field>

              <v-select v-model="form.difficulty"

                        :items="lists.difficulties.map(x =>
                         ({value: x.id, text: x.name}))" label="Difficulty"

                        @change="selectDifficulty"></v-select>

              <v-select v-model="form.categories" chips deletable-chips label="Categories" multiple small-chips

                        :items="lists.categories.map(x =>
                         ({value: x.id, text: x.name}))"

                        @change="selectCategories"></v-select>

              <v-select v-model="form.prerequisites" chips deletable-chips label="Prerequisites" multiple small-chips

                        :items="lists.tricks
                        .filter(x => x.id !== form.id || form.id === undefined)
                        .map(x => ({value: x.id, text: x.name}))"

                        @change="selectPrerequisites"></v-select>

              <v-select v-model="form.progressions" chips deletable-chips label="Progressions" multiple small-chips

                        :items="lists.tricks
                        .filter(x => x.id !== form.id || form.id === undefined)
                        .map(x => ({value: x.id, text: x.name}))"

                        @change="selectProgressions"></v-select>

              <div class="d-flex justify-center">
                <v-btn @click="step++">Next</v-btn>
              </div>
            </div>
          </v-stepper-content>

          <v-stepper-content step="2">
            <div>
              <v-btn @click="save">Save</v-btn>
            </div>
          </v-stepper-content>
        </v-stepper-items>
      </v-stepper>
    </v-card>

  </div>
</template>

<script>
import {mapActions, mapState} from 'vuex';
import {close} from "@/components/content-creation/_shared";

export default {
  name: "trick-steps",
  mixins: [close],
  data: () => ({
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
  ,
  created() {
    if (this.editing) {
      Object.assign(this.form, this.editPayload)
      console.log("form", this.form)
    }
  },
  computed: {
    ...mapState('video-upload', ["editing", "editPayload"]),
    ...mapState('tricks', ['lists']),
  },
  methods: {
    ...mapActions('tricks', ['createTrick', 'updateTrick']),
    async save() {
      if (this.editing) {
        await this.updateTrick({form: this.form});
      } else {
        await this.createTrick({form: this.form});
      }
      this.close();
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

