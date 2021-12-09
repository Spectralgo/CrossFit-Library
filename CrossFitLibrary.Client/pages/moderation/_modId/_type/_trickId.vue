<template>
  <div>
    <div v-if="item">
      {{ item.description }}
    </div>
    <!--    <div>-->
    <!--      <div v-if="parentId > 0">-->
    <!--        Replying to: {{ parentId }}-->
    <!--        <v-btn @click="parentId = 0">Clear</v-btn>-->
    <!--      </div>-->
    <!--      <v-text-field v-model="comment" label="Comment"></v-text-field>-->
    <!--      <v-btn @click="send">send</v-btn>-->
    <!--    </div>-->
    <!--    <div v-for="c in comments" class="my-1">-->
    <!--      <span v-html="c.htmlContent"></span>-->
    <!--      <v-btn @click="parentId = c.id">Reply</v-btn>-->
    <!--  changing parentId to a trick instead of a comment-->
    <!--      <v-btn @click="loadReplies(c)">Load Replies</v-btn>-->
    <!--      <div v-for="r in c.replies">-->
    <!--        {{r.htmlContent}}-->
    <!--      </div>-->
    <!--    </div>-->
    <div v-if="trickId">
      <Comments v-bind:targetId="trickId" v-bind:type="type"/>
    </div>
  </div>


</template>

<script>
import Comments from '@/components/comments/comment-section.vue'

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

  components: {
    Comments,
  },
  data() {
    return {
      item: null,
      comments: [],
      comment: "",
      parentId: "",
      modId: null,
      type: null,
      trickId: null,
    }
  },
  beforeCreate() {

    // console.log('before create modId', this.modId)
  },
  created() {

    this.trickId = this.$route.params.trickId
    this.modId = this.$route.params.modId
    this.type = this.$route.params.type

    const endpoint = endpointResolver(this.type)
    this.$axios.$get(`api/${endpoint}/${this.trickId}`).then((item) => this.item = item)
    // this.$axios.$get(`api/moderation-items/${modId}/comments`)
    // .then((comments) => this.comments = comments.map(commentWithReplies))
    // this.$axios.$get(`api/comments/${this.parentId}/tricks`).then((comments) => this.comments = comments)

    // console.log('create modId', this.modId)
  },
  computed: {},
  methods: {
    send() {
      // const {modId, trickId} = this.$route.params;
      const data = {content: this.comment}
      this.parentId = trickId

      // if (this.parentId > 0) {
      //   this.$axios.$post(`api/comments/${this.parentId}/replies`, data)
      //     .then((comment) => this.comments
      //       .find(x => x.id === this.parentId)
      //       .replies
      //       .push(comment))
      // } else {
      //   this.$axios.$post(`api/moderation-items/${modId}/comments`, data).then((comment) => this.comments.push(commentWithReplies(comment)))
      // }
      this.$axios.$post(`api/comments/${this.parentId}/tricks`, data)
        .then((comment) => this.comments
          .push(comment))
      this.comment = ""
    },
    async loadReplies(comment) {
      comment.replies = await this.$axios.$get(`api/comments/${comment.id}/replies`)
    },
  }
}
</script>

<style scoped>

</style>
