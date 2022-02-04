<template>
  <div>
    <button
      class="btn btn-primary mb-50"
      @click="changePage(currentPage - 1, itemsCount)"
      :disabled="currentPage === 1"
    >
      previus tests
    </button>

    <button 
    class="btn btn-primary mb-50 mx-button-2"
    v-for="number in getRangeButtons(currentPage, totalPages)" 
    :key="number"
    @click="changePage(number, itemsCount)"
    :disabled="currentPage === number"
    >
    {{number}}
    </button>

    <button
      class="btn btn-primary mb-50"
      @click="changePage(currentPage + 1, itemsCount)"
      :disabled="currentPage === totalPages"
    >
      next tests
    </button>
  </div>
</template>

<script>
export default {
  props: ["currentPage", "totalPages", "itemsCount"],
  methods: {
    changePage(pageNumber, pageSize) {
      this.$emit("changePageEvent", { pageNumber, pageSize });
    },

    getRangeButtons(current,last) {
      var delta = 2,
        left = current - delta,
        right = current + delta + 1,
        range = [];
        
      for (let i = 1; i <= last; i++) {
        if (i == 1 || i == last || (i >= left && i < right)) {
          range.push(i);
        }
      }
      return range;
    },
  },
};
</script>

<style>
.mt-50 {
  margin-top: 50px;
}
.mx-button-2{
margin-left: 2px;
margin-right: 2px;
}
</style>