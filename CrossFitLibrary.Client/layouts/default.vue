<template>
  <v-app dark>
    <div>
      <v-app-bar app dense>

        <v-toolbar-title>Crossfit Tricks</v-toolbar-title>

        <v-spacer></v-spacer>

        <ContentCreationDialog/>

      </v-app-bar>
    </div>
    <v-main>
      <Nuxt/>
    </v-main>
  </v-app>
</template>

<script>
import ContentCreationDialog from "../components/content-creation/content-creation-dialog";

export default {
components: {ContentCreationDialog}
}
</script>
