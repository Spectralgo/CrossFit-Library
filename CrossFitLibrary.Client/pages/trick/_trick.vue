<template>
  <ItemContentLayout>
    <template v-slot:content>
      <div v-if="submissions">
        <Submission v-for="s in submissions" :key="`submission-${s.id}`" :submission="s"/>
      </div>
    </template>
    <template v-slot:item="{close}">
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
      <v-divider class="my-1"></v-divider>
      <div>
        <v-btn outlined small @click="edit(); close();">edit</v-btn>
      </div>

    </template>
  </ItemContentLayout>
</template>

<script>
import {mapMutations, mapState} from 'vuex';
import ItemContentLayout from '../../components/item-content-layout';
import TrickSteps from "@/components/content-creation/trick-steps";
import Submission from "@/components/submission";

export default {
  components: {
    ItemContentLayout,
    Submission
  },
  data: () => ({

    trick: null,
    difficulty: null,
  }),
  methods: {
    ...mapMutations("video-upload", ["activate"]),
    edit() {
      this.activate({
        component: TrickSteps,
        edit: true,
        editPayload: this.trick
      })
    }
  },
  computed: {
    ...mapState('submissions', ['submissions']),
    ...mapState('tricks', ['dictionary']),
    relatedData() {
      return [
        {
          title: "Categories",
          data: this.trick.categories.map(x => this.dictionary.categories[x]),
          idFactory: c => `category-${c.id}`,
          routeFactory: c => `/category/${c.id}`,
        },
        {
          title: "Prerequisites",
          data: this.trick.prerequisites.map(x => this.dictionary.tricks[x]),
          idFactory: t => `trick-${t.id}`,
          routeFactory: t => `/trick/${t.slug}`,
        },
        {
          title: "Progressions",
          data: this.trick.progressions.map(x => this.dictionary.tricks[x]),
          idFactory: t => `trick-${t.id}`,
          routeFactory: t => `/trick/${t.slug}`,
        },
      ]
    },
  },
  async fetch() {
    const trickSlug = this.$route.params.trick
    this.trick = this.dictionary.tricks[trickSlug]
    this.difficulty = this.dictionary.difficulties[this.trick.difficulty]
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
