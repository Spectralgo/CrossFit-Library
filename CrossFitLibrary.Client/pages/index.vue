<template>
  <div>
    <div>
      <v-btn @click="login">Login</v-btn>
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
import {UserManager, WebStorageStateStore } from 'oidc-client';

export default {
  data: () => ({
    userMgr: null,
  }),
  created() {
    if (!process.server) {
      this.userMgr = new UserManager({
        authority: "http://localhost:5000",
        client_id: "crossfit-library-client",
        redirect_uri: "http://localhost:3000",
        response_type: "code",
        scope: "openid profile",
        post_logout_redirect_uri: "http://localhost:3000",
        // silent_redirect_uri: "http://localhost:3000",
        userStore: new WebStorageStateStore({store: window.localStorage})
      })

      const {code, scope, session_state, state} = this.$route.query

      if(code && scope && session_state && state){
        this.userMgr.signinRedirectCallback().then(user => console.log(user))
        this.$router.push('/')
      }

    }
  },
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
       return this.userMgr.signinRedirect()
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
