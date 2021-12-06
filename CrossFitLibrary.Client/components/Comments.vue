<template>
  <div>
    <v-card class="mx-auto pa-2" width="500">
      <v-text-field v-model="comment" append-icon="mdi-send" clear-icon="mdi-close-circle" icon label="Comment" outlined
                    type="text" @click:append="send" @keydown.enter="send"></v-text-field>

      <div v-if="comments.length > 0">
        <div v-for="c in comments.slice().reverse()" class="py-2">

          <v-card class="ml-5 pb-2" elevation="1" max-width="400">
            <v-card-actions class="py-0 pl-0">
              <v-list-item class="grow ">
                <v-list-item-avatar color="grey darken-3" size="20">

                  <v-img alt="" class="elevation-6"
                         src="https://avataaars.io/?avatarStyle=Transparent&topType=ShortHairShortCurly&accessoriesType=Prescription02&hairColor=Black&facialHairType=Blank&clotheType=Hoodie&clotheColor=White&eyeType=Default&eyebrowType=DefaultNatural&mouthType=Default&skinColor=Light"></v-img>
                </v-list-item-avatar>

                <v-list-item-content>
                  <v-list-item-subtitle class="text-h10">Evan You</v-list-item-subtitle>
                </v-list-item-content>

                <v-row align="center" justify="end">
                  <v-icon class="mr-1">
                    mdi-heart
                  </v-icon>
                  <span class="subheading mr-2">256</span>
                  <span class="mr-1">·</span>
                  <v-icon class="mr-1">
                    mdi-reply
                  </v-icon>
                  <span class="subheading">45</span>
                </v-row>
              </v-list-item>
            </v-card-actions>

            <v-card-text class="font-weight-regular py-0 ">
              <span v-html="c.htmlContent"></span>
            </v-card-text>

          </v-card>

        </div>
      </div>

    </v-card>

  </div>
</template>

<script>

const endpointResolver = (type) => {
  if (type === 'trick') return 'tricks'
}

export default {
  name: "Comments",

  props: {
    targetId: {
      type: String,
      required: true
    },
    type: {
      type: String,
      required: true
    },

  },
  data: () => ({
    comments: [],
    comment: "",
    endpoint: "",
  }),
  methods: {
    send() {
      const data = {content: this.comment}

      this.$axios.$post(`api/comments/${this.targetId}/${this.endpoint}`, data)
        .then((comment) => this.comments
          .push(comment))
      this.comment = ""
    }
  }
  ,
  created() {

    this.endpoint = endpointResolver(this.type)
    console.log("from created this endpoint", this.endpoint)
    this.$axios.$get(`api/comments/${this.targetId}/${this.endpoint}`).then((comments) => this.comments = comments)

  }


}
// id = target (trick, submission, moderationItem, comment)
</script>

<style scoped>
.gradient-color {
  opacity: 90%;
  background: linear-gradient(341deg, #06BCB2 25%, #2980b9 75%), linear-gradient(45deg, #2c3e50 25%, #2980b9 75%);
  /*background: linear-gradient(to right, #fbc2eb 0%, #a6c1ee 100%);*/
}
</style>
