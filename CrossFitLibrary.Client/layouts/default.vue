<template>
  <v-app dark>
    <div>
      <v-app-bar app dense>

        <v-toolbar-title>Crossfit Tricks</v-toolbar-title>

        <v-spacer></v-spacer>

        <content-creation-dialog/>

      </v-app-bar>
    </div>
    <v-main>
      <Nuxt/>
    </v-main>
  </v-app>
</template>

<script>
export default {}
</script>
