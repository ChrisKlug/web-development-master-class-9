<script lang="ts">
	import type { PageData } from './$types';
	export let data: PageData;
	const { product } = data;
</script>

<svelte:head>
	<title>Goods and Greens | {product.name}</title>
</svelte:head>

<div class="px-4">
	<h3>Hello {product.name}!</h3>
</div>
