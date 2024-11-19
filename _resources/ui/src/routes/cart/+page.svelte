<script lang="ts">
	import type { PageData } from './$types';
	export let data: PageData;
</script>

<svelte:head>
    <title>Goods and Greens | Shopping cart</title> 
</svelte:head>

<main class="p-4">
	<h3 class="text-2xl font-bold">Cart</h3>
	<div class="flex flex-col items-center space-y-4">
		{#if data.cart && data.cart.length > 0}
			<ul class="w-full max-w-2xl">
				{#each data.cart as item (item.productId)}
					<li class="flex items-center justify-between border-b border-gray-200 p-4">
						<div class="flex items-center space-x-4">
							<img class="h-16 w-16 object-contain" alt={item.productName} />
							<div>
								<h4 class="text-lg font-bold">{item.productName}</h4>
								<p class="text-sm text-gray-500">Description goes here</p>
							</div>
						</div>
						<div
							class="items -center
                        flex space-x-4"
						>
							<p class="text-lg font-bold">{item.count} x {item.price} SEK</p>
							<button class="text-red-500" on:click={() => console.log('remove')}>Remove</button>
						</div>
					</li>
				{/each}
			</ul>
			<div class="flex w-full max-w-2xl items-center justify-between">
				<p class="text-lg font-bold">Total:</p>
				<p class="text-lg font-bold">
					{data.cart.reduce((acc, item) => acc + item.price * item.count, 0).toFixed(2)} SEK
				</p>
			</div>
			<button
				class="max -w-2xl
            w-full rounded-md bg-green-500 p-4 font-bold text-white">Checkout</button
			>
		{:else}
			<p class="text-lg font-bold">Your cart is empty</p>
		{/if}
	</div>
</main>
