<template>
  <v-card>
    <v-card-title>Category</v-card-title>
    <v-card-text>
      <v-text-field v-model="form.name" label="Name"></v-text-field>
      <v-text-field v-model="form.description" label="Description"></v-text-field>
      <v-card-actions>
        <v-btn @click="save">Save</v-btn>
      </v-card-actions>
    </v-card-text>
  </v-card>
</template>

<script>

import {mapActions} from "vuex";

const initSate = () => ({
  form: {
    name: "",
    description: "",
  },
})

export default {

  name: "category-form",
  data: initSate,
  methods: {
    ...mapActions('tricks', ['createCategory']),
     save() {
      this.createCategory({
        form: this.form
      });
      Object.assign(this.$data, initSate())
    }
  }
}
</script>

<style scoped>

</style>
