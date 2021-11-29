<template>
    <v-card>
      <v-card-title>Create Difficulty
        <v-spacer></v-spacer>
        <v-btn @click="close" icon>
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-card-title>
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
import {close} from "@/components/content-creation/_shared";


export default {
  name: "difficulty-form",
  mixins: [close],
  data: () => ({
  form: {
    name: "",
    description: "",
  },
}),
  methods: {
    ...mapActions('tricks', ['createDifficulty']),
     save() {
       this.createDifficulty({
        form: this.form
      });
       this.close()
    }
  }
}
</script>

<style scoped>

</style>
