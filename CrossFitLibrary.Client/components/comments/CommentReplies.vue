<template>
  <div>
    <v-hover>

      <template v-slot:default="{ hover }">
        <v-card-text :class="`elevation-${hover ? 1 : 0}`" :style="`background-color: ${replyColor}`"
                     class="font-weight-regular py-0 rounded" min-width="20" @click="forceFocus"

                     v-on:mouseleave="showReplyColor($event)" v-on:mousemove="showReplyColor($event)">


          <div class="mt-2 mb-2 font-weight-bold" style="font-size: smaller ">{{ userId }}<span
            class="ml-2 font-weight-light" style="color: lightsteelblue; font-size: small">{{
              dateOfCreationCount
            }}</span></div>
          <v-row class="ml-0" style="position: relative">
            <div  style="font-size: smaller;  line-height: 1.2" v-html="reply.htmlContent"></div>
            <v-btn v-if="hover" class="ma-0" icon style="position: absolute; bottom: 5px; left: -45px">
              <v-icon> mdi-reply</v-icon>
            </v-btn>
          </v-row>
        </v-card-text>
      </template>
    </v-hover>
  </div>
</template>

<script>
export default {
  name: "comment-replies",
  props: {
    reply: {
      required: true,
      type: Object,
      default: {}

    }
  },

  data: () => ({
    hasReplies: false,
    dateOfCreationCount: "",
    replyColor: "",
    userId: "Evan You"

  }),
  methods: {
    showReplyColor(e) {
      if (e.type === 'mouseleave') {
        return this.replyColor = "white"
      }
      return this.replyColor = "#f2f5ff"

    },
    forceFocus(e) {

      console.log(e.target.style.cursor)
    }
  },
  created() {
    this.$axios.$get(
      `api/comments/${this.reply.id}/replies`
    ).then((replies) => {
      this.reply.replies = replies
      this.hasReplies = true
    });


    // Date formatting of this.comment
    const CommentDate = new Date(this.reply.dateOfCreation)
    const oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
    const oneHour = 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
    const oneMinute = 60 * 1000; // hours*minutes*seconds*milliseconds
    const now = new Date();

    //condition for the formatting
    const dateCount = Math.round(Math.abs((CommentDate - now) / oneDay));
    const hoursCount = Math.round(Math.abs((CommentDate - now) / oneHour));
    const minutesCount = Math.round(Math.abs((CommentDate - now) / oneMinute));

    let output
    if (dateCount > 0) {
      output = dateCount + "d ago"
      if (dateCount < 2) {
        output = "1d ago"
      }
    } else if (dateCount === 0 && hoursCount > 0) {
      output = hoursCount + "h ago"
      if (hoursCount < 2) {
        output = "1h ago"
      }
    } else {
      output = minutesCount + "m ago"
      if (minutesCount < 2) {
        output = "1m ago"
      }
    }
    this.dateOfCreationCount = output
  },
}
</script>

<style scoped>

</style>
