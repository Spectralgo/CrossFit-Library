<template>
  <div>
    <v-card class="mx-auto pa-2">
      <v-text-field
        v-model="comment"
        append-icon="mdi-send"
        clear-icon="mdi-close-circle"
        icon
        label="Comment"
        outlined
        type="text"
        @click:append="sendComment"
        @keydown.enter="sendComment"
      ></v-text-field>

      <div v-if="comments.length > 0">
        <div
          v-for="c in comments.slice().reverse()"
          :key="`${c.parentId}-${c.id}`"
          class="py-2"
        >
          <v-card class="ml-5 pb-2" elevation="0">
            <v-list-item class="grow">
              <v-list-item-avatar color="grey darken-3">
                <v-img
                  alt=""
                  class="elevation-6"
                  src="https://avataaars.io/?avatarStyle=Transparent&topType=ShortHairShortCurly&accessoriesType=Prescription02&hairColor=Black&facialHairType=Blank&clotheType=Hoodie&clotheColor=White&eyeType=Default&eyebrowType=DefaultNatural&mouthType=Default&skinColor=Light"
                ></v-img>
              </v-list-item-avatar>

              <v-card-text class="font-weight-regular py-0" min-width="20">
                <v-list-item-title>Evan You</v-list-item-title>

                <span v-html="c.htmlContent"></span>
              </v-card-text>

              <v-icon class="mr-1"> mdi-heart</v-icon>
              <span class="subheading mr-2">256</span>
              <span class="mr-1">
                <span @click="addComment(show)">
                  <span v-if="c.replies > 0">
                    <v-btn icon><v-icon> mdi-comment-multiple</v-icon></v-btn>
                    {{ c.replies.length }}
                  </span>
                  <span v-else>
                    <v-btn icon><v-icon> mdi-comment-plus</v-icon></v-btn>
                  </span>
                </span>
              </span>
              <!--                   <span class="subheading">45</span>-->
            </v-list-item>

            <div class="ml-16 pl-6 mr-7">
              <v-text-field
                v-if="show"
                v-model="reply"
                append-icon="mdi-send"
                clear-icon="mdi-close-circle"
                icon
                label="Reply"
                type="text"
                @click:append="sendReply(c)"
                @keydown.enter="sendReply(c)"
              ></v-text-field>
              <div v-if="c.replies > 0">
                <div v-for="r in c.replies.slice().reverse()">
                  <v-card-text class="font-weight-regular py-0" min-width="20">
                    <v-list-item-title>Evan You</v-list-item-title>

                    <span v-html="r.htmlContent"></span>
                  </v-card-text>
                </div>
              </div>
            </div>
          </v-card>
        </div>
      </div>
    </v-card>
  </div>
</template>

<script>
const getReplies = (comments) => {};

const endpointResolver = (type) => {
  if (type === "trick") return "tricks";
};

export default {
  name: "Comments",

  props: {
    targetId: {
      type: String,
      required: true,
    },
    type: {
      type: String,
      required: true,
    },
  },
  data: () => ({
    show: false,
    comments: [],
    comment: "",
    reply: "",
    endpoint: "",
  }),
  methods: {
    sendComment() {
      const data = { content: this.comment };

      this.$axios
        .$post(`api/comments/${this.targetId}/${this.endpoint}`, data)
        .then((comment) => this.comments.push(comment));
      this.comment = "";
    },
    sendReply(parentComment) {
      const data = { content: this.reply };
      console.log(parentComment);
      this.$axios
        .$post(`api/comments/${parentComment.id}/replies`, data)
        .then((reply) => parentComment.replies.push(reply)); //

      this.reply = "";
    },
    addComment(show, key) {
      console.log( key);
      this.show = !show;
    },
  },
  created() {
    this.endpoint = endpointResolver(this.type);
    console.log("from created this endpoint", this.endpoint);
    this.$axios
      .$get(`api/comments/${this.targetId}/${this.endpoint}`)
      .then((comments) => {
        this.comments = comments;
        this.comments.forEach(function (comment) {
          comment.replies = [];
        });
      }); // Todo: add replies to comments
    //Extract comments component from comment section
  },
};

// id = target (trick, submission, moderationItem, comment)
</script>

<style scoped>
.gradient-color {
  opacity: 90%;
  background: linear-gradient(341deg, #06bcb2 25%, #2980b9 75%),
    linear-gradient(45deg, #2c3e50 25%, #2980b9 75%);
  /*background: linear-gradient(to right, #fbc2eb 0%, #a6c1ee 100%);*/
}
</style>
