<template>
  <div>
    <div v-if="trick">
      <div class="text-h6">Trick: {{ trick.name }}
        <span class="ml-2">
          <v-chip :to="`/difficulty/${difficulty.id}`" small>
            {{ difficulty.name }}
          </v-chip>
        </span>
      </div>
      <v-divider class="my-1"></v-divider>
      <div class="text-body-2">{{ trick.description }}</div>
      <v-divider class="my-1"></v-divider>
      <div v-for="rd in relatedData" v-if="rd.data.length > 0">
        <div class="text-subtitle-1">{{ rd.title }}</div>
        <v-chip-group>
          <v-chip v-for="d in rd.data" :key="rd.idFactory(d)" :to="rd.routeFactory(d)" small>
            {{ d.name }}
          </v-chip>
        </v-chip-group>
      </div>
    </div>
    <v-divider class="my-2"></v-divider>
    <IfAuth>
        <template v-slot:allowed>
          <div>
            <div>
              <v-btn outlined small @click="edit(); close();">edit</v-btn>
              <v-btn outlined small @click="upload(); close();">upload</v-btn>
            </div>
            <v-divider class="my-2"></v-divider>
            <UserHeader :username="trick.user.username" :image-url="trick.user.imageUrl"
                        :append="trick.version === 1 ?'Created by':'Edited by'" :reverse="true"/>
          </div>
        </template>
        <template v-slot:forbidden="{login}">
          <v-btn small outlined @click="login">
            Log in to edit/update
          </v-btn>
        </template>
    </IfAuth>
    <!--    -->
  </div>
</template>

<script>
import {mapMutations, mapState} from "vuex";
import TrickSteps from "@/components/content-creation/trick-steps";
import SubmissionSteps from "@/components/content-creation/submission-steps";
import UserHeader from "@/components/user-header";
import IfAuth from "@/components/auth/if-auth";

export default {
  name: "trick-info-card",
  components: {IfAuth, UserHeader},
  props: {
    trick: {
      required: true,
      Type: Object,
    },
    close: {
      required: false,
      type: Function,
      defaults: () => {
      },
    }
  },
  computed: {
    ...mapState('tricks', ['dictionary']),
    relatedData() {
      return [
        {
          title: "Categories",
          data: this.trick.categories.map(x => this.dictionary.categories[x]),
          idFactory: c => `category-${c.id}`,
          routeFactory: c => `/category/${c.id}`,
        },
        {
          title: "Prerequisites",
          data: this.trick.prerequisites.map(x => this.dictionary.tricks[x]),
          idFactory: t => `trick-${t.id}`,
          routeFactory: t => `/trick/${t.slug}`,
        },
        {
          title: "Progressions",
          data: this.trick.progressions.map(x => this.dictionary.tricks[x]),
          idFactory: t => `trick-${t.id}`,
          routeFactory: t => `/trick/${t.slug}`,
        },
      ]
    },
    difficulty() {
      return this.dictionary.difficulties[this.trick.difficulty]
    },
  },
  methods: {
    ...mapMutations("video-upload", ["activate"]),
    edit() {
      this.activate({
        component: TrickSteps,
        edit: true,
        editPayload: this.trick
      })
    },
    upload() {
      this.activate({
        component: SubmissionSteps,
        setup: (form) => {
          form.trickId = this.trick.slug
        }
      })
    }
  },
}
</script>

<style scoped>

</style>
