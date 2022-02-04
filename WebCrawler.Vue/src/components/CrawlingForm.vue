<template>
  <div>
    <h1 class="elementInTheCenter">Search</h1>

    <form class="input-group" method="post" @submit.prevent="startCrawling()">
      <div class="input-group-prepend">
        <span class="input-group-text">Enter a website:</span>
      </div>
      <input id="urlField" type="url" class="form-control" name="Url" placeholder="https://www.example.com/" required v-model="input"/>
      <button id="searchButton" class="btn btn-success" UseSubmitBehavior="false" type="submit">
        Test
      </button>
    </form>
  </div>
</template>

<script>
import {eventEmitter} from '../main.js'

export default {
  data() {
    return {
      input: "",
    };
  },
  methods: {
    validateUrl(link) {
      let url;

      try {url = new URL(link);}
      catch (_) { return false;}
      return url.protocol === "http:" || url.protocol === "https:";
    },

    pushRequest(){
            this.resource.save({body: {crawlUrl: this.input}})
        .then((response) => {
            eventEmitter.$emit("crawlingFinished");
        })
    },

    startCrawling() {
        if(this.validateUrl(this.input)){
            eventEmitter.$emit("crawlingStarted");
            this.pushRequest();
        }
        else{
            window.confirm("The URL is invalid! Please enter a valid address! Template example: https://www.example.com/")
        }
      
    },
  },
  created: function () {
  this.resource = this.$resource('http://localhost:5000/api/v1/Tests')
  
  
  }
};
</script>

<style>
.elementInTheCenter {
  text-align: center;
  margin-left: auto;
  margin-right: auto;
  display: block;
}
</style>