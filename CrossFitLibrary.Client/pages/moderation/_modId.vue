<template>
  <div>
    <div v-if="modId">
      <v-row >
        <v-col cols="8">
          <v-row justify="center">
            <v-col cols="5" v-if="current">
            <TrickInfoCard class="ml-2" :trick="current"/>
            </v-col>
            <v-col cols="2" class="d-flex justify-center" v-if="current">
              <v-icon size="46">mdi-arrow-right</v-icon>
            </v-col>
            <v-col cols="5" v-if="target">
              <TrickInfoCard class="mr-2" :trick="target"/>
            </v-col>
          </v-row>
          <CommentSection v-if="commentsApiUrl" :comments-api-url="commentsApiUrl"/>
        </v-col>
        <v-col cols="4">
          <v-card>
            <v-card-title v-if="reviews > 0">Approved Reviews {{ approvedCount }} / 3)</v-card-title>
            <v-card-text>
              <div v-if="reviewApiUrl">
                <div v-for="r in reviews" :key="`review-${r.id}`">
                  <v-icon :color="reviewStatusColor(r.status)">{{ reviewStatusIcon(r.status) }}</v-icon>
                  {{ r.comment }}
                </div>

              </div>
              <div v-else>
                No Reviews
              </div>
              <v-divider class="my-2"></v-divider>

              <v-text-field v-model="comment" label="Review Comment"></v-text-field>
            </v-card-text>
            <div v-if="outdated">

              Outdated
            </div>
            <div v-else>
              <v-card-actions class="justify-center"></v-card-actions>
              <v-card-actions class="justify-center">
                <v-btn v-for="action in reviewActions" :key="`ra-${action.title}`"
                       :color="reviewStatusColor(action.status)" :disabled="action.disabled" class="white--text "
                       x-small @click="sendReview(action.status)">

                  <v-icon>
                    {{ reviewStatusIcon(action.status) }}
                  </v-icon>
                  {{ action.title }}
                </v-btn>
              </v-card-actions>
            </div>
          </v-card>
        </v-col>
      </v-row>
    </div>
  </div>


</template>

<script>
import CommentSection from '@/components/comments/comment-section.vue'
import TrickInfoCard from "@/components/trick-info-card";

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
    TrickInfoCard
  },
  data() {
    return {
      current: null,
      target: null,
      modItem: null,
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
  async created() {


    this.modId = this.$route.params.modId
    this.modItem = await this.$axios.$get(`api/moderation-items/${this.modId}`);
    this.commentsApiUrl = `api/moderation-items/${this.modId}/comments`
    this.reviewApiUrl = `api/moderation-items/${this.modId}/reviews`
    console.log("comUrl", this.commentsApiUrl)
    console.log("modItem", this.modItem)

    const endpoint = endpointResolver(this.modItem.type)
    console.log("endpoint", endpoint)

    this.$axios.$get(`api/${endpoint}/${this.modItem.current}`).then((item) => this.current = item)
    this.$axios.$get(`api/${endpoint}/${this.modItem.target}`).then((item) => this.target = item)

    this.comments = this.modItem.comments ? this.modItem.comments : [];
    this.reviews = this.modItem.reviews ? this.modItem.reviews : [];
    console.log("reviews", this.reviews)

  },
  computed: {
    reviewActions() {
      return [
        {title: "Approve", status: REVIEW_STATUS.APPROVED, disabled: false},
        {title: "Rejected", status: REVIEW_STATUS.REJECTED, disabled: !this.comment},
        {title: "Waiting", status: REVIEW_STATUS.WAITING, disabled: !this.comment}
      ]
    },
    approvedCount() {
      return this.reviews.filter(x => x.status === REVIEW_STATUS.APPROVED).length
    },
    outdated() {
      if (this.current && this.target) {
        return this.current && this.target && this.target.version - this.current.version <= 0
      }
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
