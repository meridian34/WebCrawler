<template>
  <div>
    <h1 class="elementInTheCenter">Search</h1>

    <form class="input-group"  method="post" @submit.prevent="startCrawling()">
      <div class="input-group-prepend">
        <span class="input-group-text">Enter a website:</span>
      </div>
      
      <input 
        id="urlField"
        type="url" 
        class="form-control"
        
        name="Url" 
        placeholder="https://www.example.com/"  
        v-model="input" 
        @input="$v.input.$touch"/>
        
      <button id="searchButton" class="btn btn-success" :disabled="$v.input.$invalid || !buttonIsWork" UseSubmitBehavior="false" type="submit">
        Test
      </button>
    </form>
  </div>
</template>

<script>
import {eventEmitter} from '../main.js'
import {required, url, maxLength} from 'vuelidate/lib/validators'

export default {
  data() {
    return {
      input: "",
      buttonIsWork: true
    };
  },
  methods: {
    pushRequest(){
      this.resource.save({body: {crawlUrl: this.input}})
        .then((response) => {
          eventEmitter.$emit("crawlingFinished");
        })
        .catch(e => {
          console.log(e)
          window.alert("Sorry, scanning is not available at this time. Try later")
          eventEmitter.$emit("crawlingError");
          eventEmitter.$emit("crawlingFinished");
        })
    },

    startCrawling() {
        eventEmitter.$emit("crawlingStarted");
        this.pushRequest();
        this.buttonIsWork=false
    },
  },
  created: function () {
    this.resource = this.$resource('api/v1/tests')
    
    eventEmitter.$on("crawlingStarted",()=>{
      this.buttonIsWork = false;
    })

    eventEmitter.$on("crawlingFinished",()=>{
      this.buttonIsWork = true;
    })
  },
  validations: {
    input: { 
      required, 
      url, 
      maxLength: maxLength(2083) }
  },
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