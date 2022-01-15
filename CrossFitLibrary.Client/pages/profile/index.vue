<template>
  <ItemContentLayout>
    <template v-slot:content>
      <div v-if="submissions">
        <v-card v-for="s in submissions" :key="`${s.id}`" class="my-2">
          <VideoPlayer :key="`v-${s.id}`" :video="s.video"/>
          <v-card-text>{{ s.description }}</v-card-text>
        </v-card>
      </div>
    </template>

    <template v-slot:item>
      <div v-if="profile">
        <div>
          <input class="d-none" type="file" accept="image/*" ref="profileImageInput" @change="changeProfileImage">
          <v-hover v-slot:default="{hover}">
            <v-avatar v-if="hover">
              <v-btn :disabled="uploadingImage" icon @click="$refs.profileImageInput.click()">
                <v-icon>mdi-account-edit</v-icon>
              </v-btn>
            </v-avatar>
            <v-avatar v-else-if="profile.imageUrl">
              <img :src="`${profile.imageUrl}`" alt="profile image"/>
            </v-avatar>
            <v-avatar v-else>
              <v-icon>mdi-account</v-icon>
            </v-avatar>
          </v-hover>
          {{ profile.username }}

        </div>
      </div>
    </template>
  </ItemContentLayout>

</template>

<script>
import VideoPlayer from "@/components/video-player";
import ItemContentLayout from "@/components/item-content-layout";
import {mapMutations, mapState} from "vuex";

export default {
  components: {
    VideoPlayer,
    ItemContentLayout
  },
  data: () => ({
    submissions: [],
    uploadingImage: false,
  }),
  mounted() {
    return this.$store.dispatch("auth/_watchUserLoaded", async () => {
      const profile = this.$store.state.auth.profile
      console.log('mounted profile', profile)
      this.submissions = await this.$axios.$get(`/api/users/${profile.id}/submissions`)
    })
  },
  computed: {
    ...mapState('auth', ['profile'])
  },
  methods: {
    ...mapMutations('auth', ['saveProfile']),
    changeProfileImage(event) {
      if(this.uploadingImage) return
      this.uploadingImage = true
      const inputFileElement = event.target;

      console.log(inputFileElement)
      const formData = new FormData();
      formData.append('imageFile', inputFileElement.files[0])

      return this.$axios.$put('/api/users/me/image', formData).then(profile => {
        this.saveProfile({profile})
        inputFileElement.value = ""
        this.uploadingImage = false
      })

    }
  },

}
</script>

<style scoped>

</style>
