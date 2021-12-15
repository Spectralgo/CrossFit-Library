<template>
  <div>
    <div v-if="item">
      {{ item.description }}
    </div>
    <div v-if="trickId">

      <v-row>
        <v-col cols="7">
          <CommentSection v-bind:targetId="trickId" v-bind:type="type"/>
        </v-col>
        <v-col cols="5">
          <v-card>
            <v-card-title>Reviews (0 / 3)</v-card-title>
              <v-card-text>
                <div>
                  No Reviews
                </div>
                <v-divider class="my-2"> </v-divider>

                <v-text-field label="Review Comment" v-model="reviewComment"></v-text-field>
              </v-card-text>
            <v-card-actions>
              <v-btn>Approve</v-btn>
              <v-btn>Reject</v-btn>
              <v-btn>Wait</v-btn>
            </v-card-actions>
          </v-card>
        </v-col>
      </v-row>
    </div>
  </div>


</template>

<script>
import CommentSection from '@/components/comments/comment-section.vue'

const endpointResolver = (type) => {
  if (type === 'trick') return 'tricks'
}

const REVIEW_STATUS = {
  APPROVED: 0,
  REJECTED: 1,
  WAITING: 2,
}

const reviewStatusColor = (status) => {
  if(REVIEW_STATUS.REJECTED === status) return "red"
  if(REVIEW_STATUS.WAITING === status) return "orange"
  if(REVIEW_STATUS.APPROVED === status) return "green"
}


const reviewStatusIcon = (status) => {
  if(REVIEW_STATUS.REJECTED === status) return "mdi-check"
  if(REVIEW_STATUS.WAITING === status) return "mdi-close"
  if(REVIEW_STATUS.APPROVED === status) return "mdi-time" // Todo: continue at 17:36
}



export default {

  components: {
    CommentSection,
  },
  data() {
    return {
      item: null,
      comments: [],
      reviewComment: "",
      parentId: "",
      modId: null,
      type: null,
      trickId: null,
    }
  },
  created() {

    this.trickId = this.$route.params.trickId
    this.modId = this.$route.params.modId
    this.type = this.$route.params.type

    const endpoint = endpointResolver(this.type)
    this.$axios.$get(`api/${endpoint}/${this.trickId}`).then((item) => this.item = item)
  },
  computed: {},
  methods: {
    send() {
      const data = {content: this.comment}
      this.parentId = trickId

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
