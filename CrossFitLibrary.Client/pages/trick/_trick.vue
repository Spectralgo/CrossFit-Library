<template>
  <ItemContentLayout>
    <template v-slot:content>
      <div v-if="submissions">
        <div v-for="x in 10">
          <v-card v-for="s in submissions" :key="`${x}-${trick.id}-${s.id}`" class="my-2" >
            <VideoPlayer :key="`v-${x}-${trick.id}-${s.id}`" :videoFileName="s.videoFileName"/>
            <v-card-text>
              {{ s.description }}
            </v-card-text>
          </v-card>
        </div>
      </div>
    </template>
    <template v-slot:item>
      <div v-if="trick">
        <div class="text-h6">Trick: {{ trick.name }}
          <span class="ml-2">
          <v-chip :to="`/difficulty/${difficulty.id}`" small>
            {{ difficulty.name }}
          </v-chip>
        </span>
        </div>
        <v-divider class="my-1"></v-divider>
        <div class="text-body-2">{{ trick.description }}</div>
        <v-divider class="my-1"></v-divider>
        <div v-for="rd in relatedData" v-if="rd.data.length > 0">
          <div class="text-subtitle-1">{{ rd.title }}</div>
          <v-chip-group>
            <v-chip v-for="d in rd.data" :key="rd.idFactory(d)" :to="rd.routeFactory(d)" small>
              {{ d.name }}
            </v-chip>
          </v-chip-group>
        </div>
      </div>
    </template>
  </ItemContentLayout>
</template>

<script>
import {mapGetters, mapState} from 'vuex';
import VideoPlayer from '../../components/video-player';
import ItemContentLayout from '../../components/item-content-layout';

export default {
  components: {
    VideoPlayer,
    ItemContentLayout,
  },
  data: () => ({
    trick: null,
    difficulty: null,
  }),
  computed: {
    ...mapGetters('tricks', ['trickById', "difficultyById"]),
    ...mapState('submissions', ['submissions']),
    ...mapState('tricks', ['categories', 'tricks']),
    relatedData() {
      return [
        {
          title: "Categories",
          data: this.categories.filter(x => this.trick.categories.indexOf(x.id) >= 0),
          idFactory: c => `category-${c.id}`,
          routeFactory: c => `/category/${c.id}`,
        },
        {
          title: "Prerequisites",
          data: this.tricks.filter(x => this.trick.prerequisites.indexOf(x.id) >= 0),
          idFactory: t => `trick-${t.id}`,
          routeFactory: t => `/trick/${t.id}`,
        },
        {
          title: "Progressions",
          data: this.tricks.filter(x => this.trick.progressions.indexOf(x.id) >= 0),
          idFactory: t => `trick-${t.id}`,
          routeFactory: t => `/trick/${t.id}`,
        },
      ]
    },
  },
  async fetch() {
    const trickId = this.$route.params.trick
    this.trick = this.trickById(this.$route.params.trick)
    console.log("from fetch", this.trick)
    this.difficulty = this.difficultyById(this.trick.difficulty)
    await this.$store.dispatch("submissions/fetchSubmissionsForTrick", {trickId}, {root: true});
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
