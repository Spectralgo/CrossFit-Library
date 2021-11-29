<template>
  <v-app dark>
    <div>
      <v-app-bar app dense>
        <v-toolbar-title>
<nuxt-link to="/" style="text-decoration: none" class="text-h5 text--primary">Woder</nuxt-link>
        </v-toolbar-title>

        <v-spacer></v-spacer>

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
