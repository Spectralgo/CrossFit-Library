<template>
  <ItemContentLayout>
    <template v-slot:content>
      <TrickList :tricks="tricks" />
    </template>
    <template v-slot:item>
      <div v-if="trick">
        <div class="text-h6">Difficulty: {{ difficulty.name }}</div>
        <v-divider class="my-1"></v-divider>
        <div class="text-body-2">{{ difficulty.description }}</div>
      </div>
    </template>
  </ItemContentLayout>
</template>

<script>
import {mapGetters} from 'vuex';
import TrickList from '../../components/trick-list';
import ItemContentLayout from '../../components/item-content-layout'

export default {

  components: {
    TrickList,
    ItemContentLayout
  },
  data: () => ({
    difficulty: null,
    tricks: [],
    filter: "",
  }),
  computed: {
    ...mapGetters('tricks', ['difficultyById']),
    filteredTricks() {
      if (!this.filter) return this.tricks;

      const normalizedFilter = this.filter.trim().toLowerCase();

      return this.tricks.filter(
        //implement search from data.filter value  and return filtered tricks
        (t) => {
          return t.name.toLowerCase().includes(normalizedFilter) ||
            t.description.toLowerCase().includes(normalizedFilter);
        }
      );
    },
  },
  async fetch() {
    const difficultyId = this.$route.params.difficulty;
    this.difficulty = this.difficultyById(difficultyId);
    this.tricks = await this.$axios.$get(`/api/difficulties/${difficultyId}/tricks`);
    console.log('from difficulty fetch', this.tricks);
  },
  head() {
    if (!this.difficulty) return {}

    return {
      title: this.difficulty.name,
      meta: [
        {
          hid: 'description',
          name: 'description',
          content: this.difficulty.description,
        }
      ]
    }
  }
}
</script>

<style scoped>

</style>

