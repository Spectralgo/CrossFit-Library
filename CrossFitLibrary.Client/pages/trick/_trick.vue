<template>
  <ItemContentLayout>
    <template v-slot:content>
      <div v-if="submissions">
        <Submission v-for="s in submissions" :key="`submission-${s.id}`" :submission="s"/>
      </div>
    </template>
    <template v-slot:item="{close}">
<!--     trick card-->
      <TrickInfoCard :trick="trick" :close="close"/>
    </template>
  </ItemContentLayout>
</template>

<script>
import {mapMutations, mapState} from 'vuex';
import ItemContentLayout from '../../components/item-content-layout';
import Submission from "@/components/submission";
import TrickInfoCard from "@/components/trick-info-card";

export default {
  components: {
    ItemContentLayout,
    Submission,
    TrickInfoCard
  },
  data: () => ({
    trick: null,
  }),
  computed:{
    ...mapState('tricks', ['dictionary']),
    ...mapState('submissions', ['submissions']),
  },
  async fetch() {
    const trickSlug = this.$route.params.trick
    this.trick = this.dictionary.tricks[trickSlug]
    await this.$store.dispatch("submissions/fetchSubmissionsForTrick", {trickId: trickSlug}, {root: true});
    console.log("this difficulty:", this.difficulty)
    console.log("this trick:", this.trick)
  },
  head() {
    if (!this.trick) return {}

    return {
      title: this.trick.name,
      meta: [
        {
          hid: 'description',
          name: 'description',
          content: this.trick.description,
        }
      ]
    }
  }
}
</script>

<style scoped>

</style>
