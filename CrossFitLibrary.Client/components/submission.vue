<template>
  <v-card class="my-2" >
    <UserHeader :username="submission.user.username" :image-url="submission.user.imageUrl"/>
    <VideoPlayer :video="submission.videoUrl" :thumbnail="submission.thumbnailUrl" />
    <v-card-text>
      {{ submission.description }}
    </v-card-text>
  </v-card>
</template>

<script>
import UserHeader from "@/components/user-header";
import VideoPlayer from "@/components/video-player";

export default {
  name: "submission",
  components: {UserHeader, VideoPlayer},
  props: {
    submission:{
      type: Object,
      required: true,
    }
  }

}
</script>

<style scoped>

</style>
