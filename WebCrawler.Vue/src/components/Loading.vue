<template>
  <div v-if="loadingVisible" id="loading">
      <h2 class="elementInTheCenter">Идет загрузка не паникуйте...</h2>
      <div class="loader bodyElement">
          <div class="line one"/>
          <div class="line two"/>
          <div class="line three"/>
      </div>
  </div>
</template>

<script>
import {eventEmitter} from '../main.js'

export default {
  data(){
    return{
      loadingVisible: false
    }
  },

  created() {
    eventEmitter.$on("crawlingStarted",()=>{
      this.loadingVisible = true;
    })

    eventEmitter.$on("crawlingFinished",()=>{
      this.loadingVisible = false;
    })
  }
};
</script>

<style  scoped>
.elementInTheCenter {
    text-align:center;
    margin-left : auto;
    margin-right : auto;
    display:block;
}

.bodyElement {
  background-image: radial-gradient(
    circle farthest-corner at center,
    #3c4b57 0%,
    #1c262b 100%
  );
}

.loader {
  position: relative;
  top: calc(50% - 32px);
  left: calc(50% - 32px);
  width: 64px;
  height: 64px;
  border-radius: 50%;
  perspective: 800px;
}

.line {
  position: absolute;
  box-sizing: border-box;
  width: 100%;
  height: 100%;
  border-radius: 50%;
}

.line.one {
  left: 0%;
  top: 0%;
  animation: rotate-one 1s linear infinite;
  border-bottom: 3px solid #efeffa;
}

.line.two {
  right: 0%;
  top: 0%;
  animation: rotate-two 1s linear infinite;
  border-right: 3px solid #efeffa;
}

.line.three {
  right: 0%;
  bottom: 0%;
  animation: rotate-three 1s linear infinite;
  border-top: 3px solid #efeffa;
}

@keyframes rotate-one {
  0% {
    transform: rotateX(35deg) rotateY(-45deg) rotateZ(0deg);
  }

  100% {
    transform: rotateX(35deg) rotateY(-45deg) rotateZ(360deg);
  }
}

@keyframes rotate-two {
  0% {
    transform: rotateX(50deg) rotateY(10deg) rotateZ(0deg);
  }

  100% {
    transform: rotateX(50deg) rotateY(10deg) rotateZ(360deg);
  }
}

@keyframes rotate-three {
  0% {
    transform: rotateX(35deg) rotateY(55deg) rotateZ(0deg);
  }

  100% {
    transform: rotateX(35deg) rotateY(55deg) rotateZ(360deg);
  }
}
</style>