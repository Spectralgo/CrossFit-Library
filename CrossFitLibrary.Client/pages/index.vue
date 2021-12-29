<template>
  <div>
    <div>
      <v-btn @click="api('test')">Api Test Auth</v-btn>
      <v-btn @click="api('mod')">Api Mod Auth</v-btn>
    </div>
    <div v-for="s in sections">
      <div class="d-flex flex-column align-center">
        <p class="text-h5">{{ s.title }}</p>
        <div>
          <v-btn v-for="item in s.collection" :key="`${s.title}-${item.id}`" :to="s.routeFactory(item.id)" class="mx-1">
            {{ item.name }}
          </v-btn>
        </div>
      </div>
      <v-divider class="my-5 "></v-divider>
    </div>
  </div>
</template>

<script>
import {mapState} from 'vuex';

export default {
  computed: {
    ...mapState('tricks', ['tricks', 'categories', 'difficulties']),
    sections() {
      return [
        {collection: this.tricks, title: "Tricks", routeFactory: (id) => `/trick/${id}`},
        {collection: this.categories, title: "Categories", routeFactory: (id) => `/category/${id}`},
        {collection: this.difficulties, title: "Difficulties", routeFactory: (id) => `/difficulty/${id}`},
      ]
    },
  },
  methods: {
    login() {
       return
    },
    logout() {
      return
    },
    api(x){
      return this.$axios.$get("/api/tricks/" + x)
        .then( msg => console.log(msg))

},
  }
}

</script>

<style>

.background-image {
  background-image: url("../img/pexels-adrien-olichon-2387793.jpg");
  background-size: cover;
  width: 100%;
  height: 100vh;
}
</style>
