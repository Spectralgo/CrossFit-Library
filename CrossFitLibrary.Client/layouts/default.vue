<template>
  <v-app dark>
    <div>
      <v-app-bar app dense >

        <v-toolbar-title>
          <nuxt-link class="text-h5 text--primary" style="text-decoration: none" to="/">Woder</nuxt-link>
        </v-toolbar-title>

        <v-spacer></v-spacer>
        <v-btn class="mx-1" v-if="moderator" depressed to="/moderation">Moderation</v-btn>

        <IfAuth>
          <template v-slot:allowed >
            <div class="flex d-flex align-center">
            <ContentCreationDialog/>
            <v-menu offset-y >
              <template v-slot:activator="{ on, attrs }">
                <v-btn icon v-bind="attrs" v-on="on">
                  <v-avatar  size="36">
                    <img v-if="profile.imageUrl" :src="`${profile.imageUrl}`" alt="profile image"/>
                    <v-icon v-else >mdi-account-circle</v-icon>
                  </v-avatar>
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
            </div>
          </template>
          <template v-slot:forbidden="{login}">
              <v-btn depressed outlined  @click="login">
                <v-icon left>mdi-account-circle-outline</v-icon>
                Log in
              </v-btn>
          </template>
        </IfAuth>

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
import IfAuth from "@/components/auth/if-auth";

export default {
  components: {IfAuth, ContentCreationDialog},
  computed: {
    ...mapState('auth', [ 'profile']),
    ...mapGetters('auth', [ 'moderator']),
  },

}
</script>

<style></style>
