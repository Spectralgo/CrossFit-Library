<template>
  <v-app dark>
    <div>
      <v-app-bar app dense>

        <v-toolbar-title>Crossfit Tricks</v-toolbar-title>

        <v-spacer></v-spacer>

        <v-btn depressed @click="toggleActivity">
          Upload
        </v-btn>

      </v-app-bar>
    </div>
    <video-upload/>
    <v-main>
      <Nuxt/>
    </v-main>
  </v-app>
</template>

<script>
import {mapMutations} from 'vuex';

export default {
  methods:
    mapMutations('video-upload', ['toggleActivity'])
  ,
}
</script>
