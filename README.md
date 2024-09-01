# Michi's VFX Utils
Tools for helping with VFX development.

Right now, there's actually only the "Octagon Mesh" container.

## Octagon Mesh
A custom scriptable object with an editor that allows you to create and edit simple octagon meshes.
This is useful to optimize the overdraw of 2D particles.

An octagon is a simple shape that allows to reduce the amount of "invisible" pixels without adding too much geometry,
when compared with the regular "Billboard" render mode of particle systems.
Therefore, Unity's VFX graph has an "Octagon" output module, but its regular particle system lacks that feature.

![BillboardParticleSystem.png](https://drive.usercontent.google.com/download?id=1F9wvmMv9a0KYlkcoZcRchtm5dkAyx0fT)

The easiest way to optimize a 2D billboard particle system with an octagon mesh
is to head to it's context menu and select "Michi's VFX Utils > Create Octagon Mesh".
If that option is greyed out, make sure the particle system has a material with a "main texture" from an editable folder,
because this action attempts to put the octagon mesh asset in the same place.

![CreateOctagonMesh.png](https://drive.usercontent.google.com/download?id=1d64PsLCCFRZNI2dGNJe-5J5JruYJkgtf)

This creates a new octagon mesh asset and opens it for editing in the inspector.

![OctagonMeshParticleSystem.png](https://drive.usercontent.google.com/download?id=1M6nzAuTxjeRoE5rPPyciSNxu1yMcBbG3)

You may also notice the new asset storing the editing information in the project panel.
When you expand it, you can also see the child (regular) mesh object that is edited by the octagon mesh asset.

![OctagonMeshAsset.png](https://drive.usercontent.google.com/download?id=1JdARcWe2UsAf4pNtw8kmirxqTt5O54zu)

The context menu entry also assigned this mesh to the particle system (and changed the render mode to "Mesh"),
so it's using this optimized geometry for rendering.
If you want ot use this mesh for other things as well, just assign it manually in the inspector.

![OctagonMeshAssignment.png](https://drive.usercontent.google.com/download?id=1XOj7QRL9uoVNtLCY6_0sTgW5MPSsPcIb)

If you want to create an octagon mesh for other reasons, you can just use the "Create Asset" menu item "Michi's VFX Utils > Octagon Mesh".
If you have a texture selected at that moment, it will automatically be assigned to the octagon mesh asset for previewing.