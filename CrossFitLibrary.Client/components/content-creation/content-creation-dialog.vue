<template>
  <div class="text-center">
    <v-dialog :value="active" persistent>
      <template v-slot:activator="{on}">
        <v-menu offset-y>
          <template v-slot:activator="{ on, attrs }">
            <v-btn v-bind="attrs" v-on="on">
              Create
            </v-btn>
          </template>
          <v-list>
            <v-list-item v-for="(item, index) in menuItems" :key="index">
              <v-list-item-title>
                <v-btn @click="activate(menuItems[index].component)">
                  {{ item.title }}
                </v-btn>
              </v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>

      </template>
      <component :is="component">

      </component>

      <div class="d-flex my-2 justify-center">
        <v-btn @click="reset">Close</v-btn>
      </div>

    </v-dialog>
  </div>
</template>

<script>
import {mapMutations, mapState} from 'vuex';
import TrickSteps from "./trick-steps"
import SubmissionSteps from "./submission-steps"

export default {
  name: "content-creation-dialog",
  components: {TrickSteps, SubmissionSteps},
  computed:{
    ...mapState('video-upload', ['active', 'component']),
    menuItems() {
    return [
      {component: "TrickSteps", title: "Trick"},
      {component: "SubmissionSteps", title: "Submission"}
    ]
  }
},
  methods: {
    ...mapMutations('video-upload', ['reset', 'activate']),
  }
}


</script>

<style scoped>

</style>

