<template>
  <div class="text-center">
    <v-dialog :value="active" persistent>
      <v-stepper v-model="step">
        <v-stepper-header>
          <v-stepper-step :complete="step > 1" step="1">
            Select Type
          </v-stepper-step>

          <v-divider></v-divider>
          <v-stepper-step :complete="step > 2" step="2">
            Upload
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step :complete="step > 3" step="3">
            Trick Information
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step step="4">
            Review
          </v-stepper-step>
        </v-stepper-header>

        <v-stepper-items>
          <v-stepper-content step="1">
            <div class="d-flex flex-column align-center ">
              <v-btn class="my-2" @click="setType(uploadType.TRICK)">
                Trick
              </v-btn>
              <v-btn class="my-2" @click="setType(uploadType.SUBMISSION)">Submission</v-btn>
            </div>
          </v-stepper-content>
          <v-stepper-content step="2">
            <div>
              <v-file-input accept="video/*" label="Video upload" @change="handleFile"></v-file-input>
            </div>
          </v-stepper-content>

          <v-stepper-content step="3">
            <div>
              <v-text-field v-model="trickName" label="Tricking Name"></v-text-field>
              <v-btn @click="saveTrick">Save trick</v-btn>
            </div>
          </v-stepper-content>

          <v-stepper-content step="4">
            <div>
              Success!
            </div>
          </v-stepper-content>
        </v-stepper-items>
      </v-stepper>
      <div class="d-flex my-2 justify-center">
        <v-btn @click="toggleActivity">Close</v-btn>
      </div>

    </v-dialog>
  </div>
</template>

<script>
import {mapActions, mapMutations, mapState} from 'vuex';
import {UPLOAD_TYPE} from "@/data/enum";

export default {
  name: "video-upload",
  data: () => ({trickName: ""}),

  computed: {
    ...mapState('tricks', ['tricks'],
    ),
    ...mapState('video-upload', ['uploadPromise', 'active', 'step']),
    uploadType: () =>({
      ...UPLOAD_TYPE
    })
  },
  methods: {
    ...mapMutations('tricks', {resetTricks: 'reset'}),
    ...mapMutations('video-upload', ['reset', "toggleActivity", "setType"]),
    ...mapActions('video-upload', ['startUploadingVideo', 'createTrick']),
    async handleFile(file) {
      if (!file) return;

      const form = new FormData();
      form.append('video', file);
      this.startUploadingVideo({form});

    },
    async saveTrick() {
      if (!this.uploadPromise) {
        console.log("video is null");
        return;
      }
      const video = await this.uploadPromise
      await this.createTrick({trick: {name: this.trickName, video}});
      this.trickName = "";

      this.reset();
    },;

  }
}


</script>

<style scoped>

</style>
