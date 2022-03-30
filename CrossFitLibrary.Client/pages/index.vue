<template>
  <div>
    <div v-for="s in sections">
      <div class="d-flex flex-column align-center">
        <p class="text-h5">{{ s.title }}</p>
        <div>
          <v-card class="mx-auto my-2"
                  max-width="400" v-for="item in s.collection" :key="`${s.title}-${item.slug}`" :to="s.routeFactory(item)"
                  >
            <v-img gradient="to right, rgba(030,035,038,.93), rgba(25,32,72,.3)" class="white--text justify-center align-center" height="200px"
                   src="https://wodinn.com/wp-content/uploads/2020/03/Matt-Fraser-Snatch-Squat-Athl%C3%A8te-Crossfit-1024x576.jpg">
              <v-card-title>
                {{ item.name }}
              </v-card-title>
            </v-img>
          </v-card>
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
    ...mapState('tricks', ['lists']),
    sections() {
      return [
        {collection: this.lists.tricks, title: "Tricks", routeFactory: (i) => `/trick/${i.slug}`},
        {collection: this.lists.categories, title: "Categories", routeFactory: (i) => `/category/${i.id}`},
        {collection: this.lists.difficulties, title: "Difficulties", routeFactory: (i) => `/difficulty/${i.id}`},
      ]
    },
  },
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
