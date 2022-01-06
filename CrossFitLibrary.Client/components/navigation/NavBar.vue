<template>
  <div>
    <v-app-bar app>
            <v-toolbar-title>Woder.app</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-app-bar-nav-icon></v-app-bar-nav-icon>
      <slot name="extension"></slot>
    </v-app-bar>
  </div>
</template>

<script>
export default {
  data: () => ({
    isNavOpen: false
  }),
  name: "NavBar"
}
</script>

<style scoped>

</style>
