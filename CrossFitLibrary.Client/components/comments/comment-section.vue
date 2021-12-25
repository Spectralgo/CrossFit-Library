<template>
  <div>
    <v-card class="mx-auto pa-2">
      <v-text-field v-model="comment" append-icon="mdi-send" clear-icon="mdi-close-circle" icon label="Add a comment"
                    outlined type="text" @click:append="sendComment" @keydown.enter="sendComment"></v-text-field>

      <div v-if="comments.length > 0">
        <div v-for="c in comments.slice().reverse()" :key="`${c.parentId}-${c.id}`" class="py-2">
          <CommentPost :comment="c" />
        </div>
      </div>
    </v-card>
  </div>
</template>

<script>
import CommentPost from '@/components/comments/CommentPost'



export default {
  name: "Comments",
  components: {
    CommentPost
  },
  props: {
    commentsApiUrl: {
      type: String,
      required: true,
    },
  },
  data: () => ({
    comments: [],
    comment: "",
    endpoint: "",
  }),
  methods: {
    sendComment() {
      const data = {content: this.comment};
      this.$axios
        .$post(this.commentsApiUrl, data)
        .then((commentWithoutReplies) =>  {
          const replies = []
          const comment = {...commentWithoutReplies, replies }
          this.comments.push(comment)
        });
      this.comment = "";
    },
  },
  created() {

    const replies = []
    const thisComments = []

    this.$axios
      .$get(this.commentsApiUrl)
      .then((comments) => {
        comments.forEach(function (comment) {
          comment = {...comment, replies}
          thisComments.push(comment)
        });

        this.comments = thisComments

      })

},
}

</script>

<style scoped>
.gradient-color {
  opacity: 90%;
  background: linear-gradient(341deg, #06bcb2 25%, #2980b9 75%),
  linear-gradient(45deg, #2c3e50 25%, #2980b9 75%);
}
</style>
