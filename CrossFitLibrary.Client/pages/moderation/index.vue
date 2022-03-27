<template>
<div>
  <v-btn  v-for="i in items" :to="`/moderation/${i.id}/${i.type}/${i.target}`"  :key="i.id">
    {{i.target}}
  </v-btn>
</div>

</template>

<script>
export default {
  data: () => ({
    items: []
  }),
  async fetch(){
      this.items = await this.$axios.$get('/api/moderation-items')
  }
}
</script>

<style scoped>

</style>
