<template>
  <div class="video-container">
    <div class="play-button" :class="{'hide' : playing}" @click="playing = !playing" >
        <v-icon size="78" >mdi-play</v-icon>
    </div>
    <video ref="video"
           :src="`http://localhost:5000/api/video/${video.videoLink}`"
           :poster="`http://localhost:5000/api/video/${video.thumbnailLink}`"
           preload="none"
           playsinline
           loop muted
    ></video>
  </div>
</template>

<script>
// take care of the localhost address
export default {
  name: "video-player",
  props: {
    video: {
      type: Object,
      required: true
    }
  },
  data: () => ({
    playing: false,

  }),
  watch: {
    playing: function (v) {
      if (v) {
        this.$refs.video.play();
      } else {
        this.$refs.video.pause();
      }
    }
  },

}
</script>

<style lang="scss" scoped>
.video-container {
  display: flex;
  position: relative;
  width: 100%;
  border-top-right-radius: inherit;
  border-top-left-radius: inherit;
  .play-button{
     //position on top of the video
    position: absolute;
    z-index: 2;
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: rgba($color: #000000, $alpha: 0.36);
    width: 100%;
    height: 100%;

    &.hide{
      opacity: 0;
    }

  }
  video {
    width: 100%;
    z-index: 1;
    border-top-right-radius: inherit;
    border-top-left-radius: inherit;
  }
}


</style>
