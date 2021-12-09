<template>
  <v-app dark>
    <div>
      <v-app-bar app dense>

        <v-toolbar-title>
          <nuxt-link class="text-h5 text--primary" style="text-decoration: none" to="/">Woder</nuxt-link>
        </v-toolbar-title>

        <v-spacer></v-spacer>
        <v-btn depressed to="/moderation">Moderation</v-btn>
        <ContentCreationDialog/>

      </v-app-bar>
    </div>
    <v-main>
      <v-container>
        <Nuxt/>
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import ContentCreationDialog from "../components/content-creation/content-creation-dialog";

export default {
  components: {ContentCreationDialog}
}
</script>

<style></style>
