<script lang="ts">
	import FeaturedCarousel from '$lib/components/FeaturedCarousel.svelte';
	import Hero from '$lib/components/Hero.svelte';
	import type { PageData } from './$types';
	export let data: PageData;
</script>

<svelte:head>
    <title>Goods and Greens</title> 
</svelte:head>

<main class="flex flex-col items-center space-y-8">
	<Hero />

	<h1 class="text-3xl font-bold">Featured Products</h1>
	<div class="mx-40 max-w-4xl">
		<FeaturedCarousel featured={data.featured} />
	</div>
</main>
