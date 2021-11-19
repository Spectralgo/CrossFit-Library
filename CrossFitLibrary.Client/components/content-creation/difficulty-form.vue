<template>
  <v-card>
    <v-card-title>Difficulty</v-card-title>
    <v-card-text>
      <v-text-field v-model="form.name" label="Difficulty"></v-text-field>
      <v-text-field v-model="form.description" label="Description"></v-text-field>
      <v-card-actions>
        <v-btn @click="save">Save</v-btn>
      </v-card-actions>
    </v-card-text>
  </v-card>
</template>

<script>
import {mapActions, mapMutations} from "vuex";

const initSate = () => ({
  form: {
    name: "",
    description: "",
  },
})

export default {
  name: "difficulty-form",
  data: initSate,
  methods: {
    ...mapActions('tricks', ['createDifficulty']),
     save() {
       this.createDifficulty({
        form: this.form
      });
      Object.assign(this.$data, initSate())
    }
  }
}
</script>

<style scoped>

</style>
