<template>
  <div>
    <v-card>
      <v-file-input
        accept="video/*"
        label="Video upload"
        @change="handleFile"
      ></v-file-input>
    </v-card>
  </div>
</template>

<script>
import {mapActions, mapMutations, mapState} from 'vuex';

export default {
  data: () => ({trickName: ""}),

  computed: {
    ...mapState({
      message: state => state.message

    }),
    ...mapState('tricks', {
      tricks: state => state.tricks

    }),

  },
  methods: {
    ...mapMutations(['reset']),
    ...mapMutations('tricks', {resetTricks: 'reset'}),
    ...mapActions('tricks', ['createTrick']),
    async saveTrick() {
      await this.createTrick({trick: {name: this.trickName}});
      this.trickName = "";
    },
    async handleFile(file){
      if (!file) return;

      console.log(file)
    }
  }
  // async fetch(){
  //   await this.$store.dispatch('fetchMessage')
  // }

}

</script>
