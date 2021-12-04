<template>
  <div>
    <div v-if="item">
      {{ item.description }}
    </div>
    <div>
      <div v-if="parentId > 0">
        Replying to: {{ parentId }}
        <v-btn @click="parentId = 0">Clear</v-btn>
      </div>
      <v-text-field v-model="comment" label="Comment"></v-text-field>
      <v-btn @click="send">send</v-btn>
    </div>
    <div v-for="c in comments" class="my-1">
      <span v-html="c.htmlContent"></span>
      <v-btn @click="parentId = c.id">Reply</v-btn>
      <v-btn @click="loadReplies(c)">Load Replies</v-btn>
      <div v-for="r in c.replies">
        {{r.htmlContent}}
      </div>
    </div>
  </div>

</template>

<script>

const endpointResolver = (type) => {
  if (type === 'trick') return 'tricks'
}

const commentWithReplies = (comment) => {
  return {
    ...comment,
    replies: [],
  }
}

export default {
  data: () => ({
    item: null,
    comments: [],
    comment: "",
    parentId: 0, // We use this id to find the id of the target comment in EF Core to add this comment to its list of replies
  }),
  created() {
    const {modId, type, trickId} = this.$route.params
    const endpoint = endpointResolver(type)
    this.$axios.$get(`api/${endpoint}/${trickId}`).then((item) => this.item = item)
    this.$axios.$get(`api/moderation-items/${modId}/comments`)
      .then((comments) => this.comments = comments.map(commentWithReplies))


  },
  methods: {
    send() {
      const {modId} = this.$route.params;
      const data = {content: this.comment}

      if (this.parentId > 0) {
        this.$axios.$post(`api/comments/${this.parentId}/replies`, data)
          .then((comment) => this.comments
            .find(x => x.id === this.parentId)
            .replies
            .push(comment))
      } else {
        this.$axios.$post(`api/moderation-items/${modId}/comments`, data).then((comment) => this.comments.push(commentWithReplies(comment)))
      }
      this.comment = ""
    },
    async loadReplies(comment){
      comment.replies = await this.$axios.$get(`api/comments/${comment.id}/replies`)
    }
  }
}
</script>

<style scoped>

</style>
