<template>
  <v-card
    class="mx-auto"
    max-width="400"
  >
      <v-card-title>
        <v-avatar>
          <v-icon>mdi-account</v-icon>
        </v-avatar> Test user
      </v-card-title>

    <v-card-text class="text--primary">
      <div v-if="submissions">
          <v-card v-for="s in submissions" :key="`${s.id}`" class="my-2" >
            <VideoPlayer :key="`v-${s.id}`" :video="s.video"/>
            <v-card-text>{{ s.description }}</v-card-text>
          </v-card>
      </div>
    </v-card-text>
  </v-card>
</template>

<script>
import VideoPlayer from "@/components/video-player";

export default {
  components: {
    VideoPlayer
  },
  data: () => ({
    submissions: []
  }),
  mounted(){
    return this.$store.dispatch("auth/_watchUserLoaded", async () => {
      const profile = this.$store.state.auth.profile
      console.log('mounted profile', profile)
      this.submissions = await this.$axios.$get(`/api/users/${profile.id}/submissions`)
    })
  }
}
</script>

<style scoped>

</style>
