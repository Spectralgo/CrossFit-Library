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
          <div class="text-center">
            <v-menu offset-y v-if="authenticated">
              <template v-slot:activator="{ on, attrs }">
                <v-btn icon v-bind="attrs" v-on="on">
                  <v-avatar v-if="profile.imageUrl" size="36">
                    <img :src="`${profile.imageUrl}`" alt="profile image"/>
                  </v-avatar>
                  <v-icon v-else >mdi-account-circle</v-icon>
                </v-btn>
              </template>
              <v-list>
                <v-list-item @click="$router.push('/profile')">
                  <v-list-item-title>Profile</v-list-item-title>
                </v-list-item>
                <v-list-item @click="$auth.signoutRedirect()">
                  <v-list-item-title>Logout</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
          <v-btn depressed outlined v-else @click="login">
            <v-icon left>mdi-account-circle-outline</v-icon>Log in</v-btn>
          </div>
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
import {mapActions, mapGetters, mapState} from "vuex";

export default {
  components: {ContentCreationDialog},
  computed: {
    ...mapState('auth', ['loading', 'profile']),
    ...mapGetters('auth', ['authenticated', 'moderator']),
  },
  methods: mapActions('auth', ['login'])

}
</script>

<style></style>
