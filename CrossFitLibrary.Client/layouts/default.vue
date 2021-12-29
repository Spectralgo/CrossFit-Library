<template>
  <v-app dark>
    <div>
      <v-app-bar app dense>

        <v-toolbar-title>
          <nuxt-link class="text-h5 text--primary" style="text-decoration: none" to="/">Woder</nuxt-link>
        </v-toolbar-title>

        <v-spacer></v-spacer>
        <v-btn class="mx-1" v-if="moderator" depressed to="/moderation">Moderation</v-btn>

        <v-skeleton-loader class="mx-1" :loading="loading"  type="button">
          <ContentCreationDialog/>
        </v-skeleton-loader>

        <v-skeleton-loader class="mx-1" :loading="loading"  type="button">
          <v-btn depressed outlined v-if="authenticated">
            <v-icon left>mdi-account-circle</v-icon>
            Profile
          </v-btn>


          <v-btn depressed outlined v-else @click="$auth.signinRedirect()">
            <v-icon left>mdi-account-circle-outline</v-icon>sign in</v-btn>

          <v-btn v-if="authenticated" depressed  @click="$auth.signoutRedirect()">Logout</v-btn>
        </v-skeleton-loader>

      </v-app-bar>
    </div>
    <v-main>
      <v-container v-if="$vuetify.breakpoint.smOnly">
        <Nuxt/>
      </v-container>
        <Nuxt v-else/>
    </v-main>
  </v-app>
</template>

<script>
import ContentCreationDialog from "../components/content-creation/content-creation-dialog";
import {mapGetters, mapState} from "vuex";

export default {
  components: {ContentCreationDialog},
  computed: {
    ...mapState('auth', ['loading']),
    ...mapGetters('auth', ['authenticated', 'moderator']),
  }

}
</script>

<style></style>
