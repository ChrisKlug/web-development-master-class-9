<script lang="ts">
	import * as Carousel from '$lib/components/ui/carousel';
	import type { Product } from '$lib/types/Product';
	import ProductCard from './ProductCard.svelte';

	let { featured }: { featured: Product[] } = $props();
</script>

<Carousel.Root
	opts={{
		align: 'start'
	}}
	class="w-full"
>
	<Carousel.Content class="items-stretch">
		{#each featured as product}
			<Carousel.Item class="md:basis-1/2 lg:basis-1/3 xl:basis-1/4">
				<div class="h-full p-1">
					<ProductCard {product} />
				</div>
			</Carousel.Item>
		{/each}
	</Carousel.Content>
	<Carousel.Previous />
	<Carousel.Next />
</Carousel.Root>
