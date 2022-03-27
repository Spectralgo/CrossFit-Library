<template>
  <div>
    <div v-if="item">
      {{ item.description }}
    </div>
    <div v-if="trickId">

      <v-row>
        <v-col cols="7">
          <CommentSection :comments-api-url="commentsApiUrl"/>
        </v-col>
        <v-col cols="5">
          <v-card>
            <v-card-title>Approved Reviews {{approvedCount}} / 3)</v-card-title>
            <v-card-text>
              <div v-if="reviews.length > 0">
                <div v-for="r in reviews" :key="`review-${r.id}`">
                  <v-icon :color="reviewStatusColor(r.status)">{{reviewStatusIcon(r.status)}}</v-icon>
                  {{r.comment}}
                </div>

              </div>
              <div v-else>
                No Reviews
              </div>
              <v-divider class="my-2"></v-divider>

              <v-text-field v-model="comment" label="Review Comment"></v-text-field>
            </v-card-text>
            <v-card-actions class="justify-center">
              <v-btn v-for="action in reviewActions" :key="`ra-${action.title}`" :color="reviewStatusColor(action.status)"
                     :disabled="action.disabled" class="white--text " x-small
                     @click="sendReview(action.status)">

                <v-icon>
                  {{ reviewStatusIcon(action.status) }}
                </v-icon>
                {{ action.title }}
              </v-btn>
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
  if (REVIEW_STATUS.REJECTED === status) return "red"
  if (REVIEW_STATUS.WAITING === status) return "orange"
  if (REVIEW_STATUS.APPROVED === status) return "green"
}


const reviewStatusIcon = (status) => {
  if (REVIEW_STATUS.REJECTED === status) return "mdi-close"
  if (REVIEW_STATUS.WAITING === status) return "mdi-clock"
  if (REVIEW_STATUS.APPROVED === status) return "mdi-check"
}


export default {

  components: {
    CommentSection,
  },
  data() {
    return {
      item: null,
      comments: [],
      reviews: [],
      comment: "",
      parentId: "",
      modId: null,
      type: null,
      trickId: null,
      commentsApiUrl: null,
      reviewApiUrl: null,
    }
  },
  created() {

    this.modId = this.$route.params.modId
    this.commentsApiUrl = `api/moderation-items/${this.modId}/comments`
    this.reviewApiUrl = `api/moderation-items/${this.modId}/reviews`

    console.log(this.commentsApiUrl)

    const endpoint = endpointResolver(this.type)
    this.$axios.$get(`api/${endpoint}/${this.trickId}`).then((item) => this.item = item)

    this.$axios.$get(this.commentsApiUrl).then((comments) => this.comments = comments)

    this.$axios.$get(this.reviewApiUrl).then((reviews) => this.reviews = reviews)
  },
  computed: {
    reviewActions() {
      return [
        {title: "Approve", status: REVIEW_STATUS.APPROVED, disabled: false},
        {title: "Rejected", status: REVIEW_STATUS.REJECTED, disabled: !this.comment},
        {title: "Waiting", status: REVIEW_STATUS.WAITING, disabled: !this.comment}
      ]
    },
    approvedCount(){
      return this.reviews.filter(x => x.status === REVIEW_STATUS.APPROVED).length
    }
  },
  methods: {
    reviewStatusColor,
    reviewStatusIcon,

    sendComment() {
      const data = {content: this.comment}
      this.$axios.$post(this.commentsApiUrl, data)
        .then((comment) => this.comments
          .push(comment))
      this.comment = ""
    },
    sendReview(status) {
      const data = {
        comment: this.comment,
        status: status
      }
      this.$axios.$post(this.reviewApiUrl, data)
        .then((review) => this.reviews
          .push(review))
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
