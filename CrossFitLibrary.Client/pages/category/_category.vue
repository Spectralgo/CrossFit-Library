<template>
  <ItemContentLayout>
    <template v-slot:content>
      <TrickList :tricks="tricks"/>
    </template>
    <template v-slot:item>
      <div v-if="category">
        <div class="text-h6">Category: {{ category.name }}</div>
        <v-divider class="my-1"></v-divider>
        <div class="text-body-2">{{ category.description }}</div>
      </div>
    </template>
  </ItemContentLayout>
</template>

<script>
import {mapGetters} from 'vuex';
import TrickList from '../../components/trick-list'
import ItemContentLayout from '../../components/item-content-layout'

export default {
  components: {
    TrickList,
    ItemContentLayout
  },
  data: () => ({
    category: null,
    tricks: [],
  }),
  computed: {
    ...mapGetters('tricks', ['categoryById']),
  },
  async fetch() {
    const categoryId = this.$route.params.category;
    this.category = this.categoryById(categoryId);
    this.tricks = await this.$axios.$get(`/api/categories/${categoryId}/tricks`);
    console.log('from category fetch', this.tricks);
  },
  head() {
    if (!this.category) return {}

    return {
      title: this.category.name,
      meta: [
        {
          hid: 'description',
          name: 'description',
          content: this.category.description,
        }
      ]
    }
  }
}
</script>

<style scoped>

</style>
