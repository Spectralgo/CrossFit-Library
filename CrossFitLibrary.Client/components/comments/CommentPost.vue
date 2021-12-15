<template>
  <div v-if="comment">
    <v-card class="ml-5 pb-2" elevation="0">
      <div class="d-flex justify-center align-start pa-0" >
        <v-avatar class="mt-0 mr-2" color="grey darken-3" >
          <v-img alt=""
                 src="https://avataaars.io/?avatarStyle=Transparent&topType=ShortHairShortCurly&accessoriesType=Prescription02&hairColor=Black&facialHairType=Blank&clotheType=Hoodie&clotheColor=White&eyeType=Default&eyebrowType=DefaultNatural&mouthType=Default&skinColor=Light"></v-img>
        </v-avatar>

          <v-card-text class="font-weight-regular ma-0 pa-0"  v-on:click.self="showReplies($event)"
                     v-on:mousedown="getMousedown($event)" v-on:mouseup="isTextSelection($event)">
          <v-list-item-title> <span class="font-weight-bold">
            Evan You</span> <span class="mr-2 font-weight-light" style="color: lightsteelblue">{{
              dateOfCreationCount
            }}</span></v-list-item-title>
          <div v-on:click.self="showReplies($event)" style="font-size: smaller; line-height: 1.2" v-html="comment.htmlContent"></div>
        <div>
        <v-chip icon color="white" class="mr-2" ><v-icon  color="grey" size="15" class="mr-1 mb-1">mdi-thumb-up</v-icon>{{"205"}}</v-chip>
        <span >
          <span @click="addComment">
            <span v-if="comment.replies.length > 0">

              <v-chip icon color="white" ><v-icon color="grey" size="20" class="mr-1"> mdi-comment-multiple</v-icon>{{ comment.replies.length }}</v-chip>

            </span>
            <span v-else>
              <v-chip color="white"  icon> <v-icon color="grey" size="20" >mdi-comment-text</v-icon></v-chip>
            </span>
          </span>
        </span>
        </div>
        </v-card-text>
      </div>

      <div class="ml-6 pl-6 mr-7">
        <v-text-field v-if="show" ref="textFieldInput"
                      v-model="reply" append-icon="mdi-send" clear-icon="mdi-close-circle"
                      icon label="Reply" type="text"
                      @click:append="sendReply" @keydown.enter="sendReply"></v-text-field>
        <div v-if="show">
          <div v-for="r in comment.replies.slice().reverse()" :key="`${r.parentId}-${r.id}`" >
            <CommentReplies v-on:forcefocus="focusWithUserId($event)" :reply="r" class="mb-3"/>

          </div>
        </div>
      </div>
    </v-card>
  </div>
</template>

<script>
import CommentReplies from "@/components/comments/CommentReplies";
import CommentsReplyIcon from "assets/icons/comments-reply-icon";

export default {
  name: "CommentPost",
  components: {
    CommentsReplyIcon,
    CommentReplies
  },
  emit: ["sendReply"],
  data: () => ({
    reply: "",
    show: false,
    hasReplies: true,
    dateOfCreationCount: "",
    mousedownTimeStamp: "",
    isTextSelected: false,
    cancelClick: false,
  }),
  props: {
    comment: {
      required: true,
      type: Object,
      default: {
        replies: []
      },
    },
  },
  computed: {},
  methods: {
    addComment() {
      this.show = !this.show;
    },
    focusWithUserId(userId) {
      console.log("I fired focus with user Id")
      this.$refs.textFieldInput.focus()
      this.reply += userId + " "
    },
    sendReply() {
      const data = {content: this.reply};
      this.$axios.$post(`api/comments/${this.comment.id}/replies`, data).then((reply) => {
        this.comment.replies.push(reply);
      });
      this.reply = "";
      this.userIdReply = "";
      this.hasReplies = true;
    },
    getMousedown(e) {

      this.mousedownTimeStamp = e.timeStamp

      if (this.isTextSelected === true) return this.cancelClick = true
      this.cancelClick = false
    },
    isTextSelection(e) {
      this.isTextSelected = false
      const result = window.getSelection().toString().trim()
      if (result.length > 0) {
        this.isTextSelected = true
      }

    },
    showReplies(e) {
      if (this.cancelClick === true) return !this.cancelClick

      if (e.timeStamp - this.mousedownTimeStamp < 1000 && this.isTextSelected === false) {
        this.show = !this.show
      }

    },
  },
  created() {
    //Getting the replies of the this.comment (form prop)
    this.$axios.$get(
      `api/comments/${this.comment.id}/replies`
    ).then((replies) => {
      this.comment.replies = replies
    });

    // Date formatting of this.comment
    const CommentDate = new Date(this.comment.dateOfCreation)
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
      output = dateCount + " days ago"
      if (dateCount < 2) {
        output = "One day ago"
      }
    } else if (dateCount === 0 && hoursCount > 0) {
      output = hoursCount + " hours ago"
      if (hoursCount < 2) {
        output = "One hour ago"
      }
    } else {
      output = minutesCount + " minutes ago"
      if (minutesCount < 2) {
        output = "One minute ago"
      }
    }
    this.dateOfCreationCount = output

  },
};
</script>

<style scoped></style>
