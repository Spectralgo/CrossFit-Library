<template>
  <v-row>
    <v-col cols="12" lg="6" offset-lg="1" offset-xl="3" sm="7" xl="4">
      <slot name="content"></slot>
    </v-col>
    <v-col class="d-none d-sm-block" lg="4" sm="5" xl="3">
      <v-sheet class="pa-3 sticky">
      <slot name="item" :close="() => {}"></slot>
      </v-sheet>
    </v-col>
    <v-dialog v-model="dialog">
      <template v-slot:activator="{on}">
        <v-btn bottom class="d-sm-none" color="blue" fab fixed right small v-on="on">
          <v-icon>
            mdi-help-circle-outline
          </v-icon>
        </v-btn>
      </template>
      <v-sheet class="pa-2 rounded">
      <slot name="item" :close="() => dialog = false"></slot>
        </v-sheet>
    </v-dialog>
  </v-row>
</template>

<script>
export default {
  name: "item-content-layout",
  data: () => ({
    dialog: false
  })
}
</script>

<style scoped>

</style>
