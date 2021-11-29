<template>
  <v-card>
    <v-card-title>Create Category
      <v-spacer></v-spacer>
      <v-btn @click="close" icon>
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </v-card-title>
    <v-card-text>
      <v-text-field v-model="form.name" label="Name"></v-text-field>
      <v-text-field v-model="form.description" label="Description"></v-text-field>
      <v-card-actions class="d-flex justify-center">
        <v-btn  @click="save">Save</v-btn>
      </v-card-actions>
    </v-card-text>
  </v-card>
</template>

<script>

import {mapActions} from "vuex";
import {close} from "@/components/content-creation/_shared";


export default {

  name: "category-form",
  mixins: [close],
  data: () => ({
    form: {
      name: "",
      description: "",
    },
  }),
  methods: {
    ...mapActions('tricks', ['createCategory']),
     save() {
      this.createCategory({
        form: this.form
      });
      this.close()
    }
  }
}
</script>

<style scoped>

</style>
