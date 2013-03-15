// ============================================================================
// File: DoNothing.vs
// ----------------------------------------------------------------------------
// Description: Vertex shader used to keep screen coordinates projected.
// Author: Alejandro Piad <alepiad@gmail.com>
// ----------------------------------------------------------------------------

struct VS_IN
{
	float3 pos : POSITION;
};

struct PS_IN
{
	float4 proj : SV_POSITION;
};

PS_IN VS( VS_IN input )
{
	// Create the final structure
	PS_IN output = (PS_IN)0;
	
	// Set the output
	output.proj = float4(input.pos, 1);

	// Just like that...
	return output;
}