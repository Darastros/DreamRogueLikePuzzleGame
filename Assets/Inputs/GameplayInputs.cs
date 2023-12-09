//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Inputs/Gameplay.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @GameplayInputs: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameplayInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Gameplay"",
    ""maps"": [
        {
            ""name"": ""TopDown"",
            ""id"": ""e984c9c4-72e5-415d-a911-2bd0fdcc8fcb"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""16908430-cfdd-406f-8bb6-5bf751e6ce20"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Craft"",
                    ""type"": ""Button"",
                    ""id"": ""da6ade11-ef57-48c3-aa92-958fd2b5e43f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1cf65531-7b96-4b20-93fb-2419267c9db1"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""D-Pad"",
                    ""id"": ""88c40410-6703-431a-9361-046c34c076d5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e27ca1e5-add4-4d28-85ca-d30d21ff96d2"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ab82ec3d-4bc7-48cd-888c-cf4cd45be301"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""92024751-636c-4725-866a-e1ef33668730"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""635293da-8693-41b6-9b01-ccd8074414dc"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""754888f8-b154-41e6-89e7-6fd0475089e6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""926054e0-0ba8-421e-a47d-ebe9927586c6"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""cdeaedfc-14db-4f41-8bf1-237e67fb0da6"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e253395b-c6db-4c8c-9dc4-55241b673f43"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3b54a9db-175d-469d-b7fc-2726ce256553"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1865700d-d058-45ba-a031-9ee3d4382daa"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Craft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0692a66f-3c6b-4b4a-96d7-a42f4867de75"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Craft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Platformer"",
            ""id"": ""4c91869a-58f4-4fe9-a36a-1abd1fa0e156"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""c4dd71e8-5f20-401b-8c0b-726b2584a6e3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7c5365c2-de28-4e57-801e-b500d7d0046b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Craft"",
                    ""type"": ""Button"",
                    ""id"": ""0bdfdff3-1443-479c-a449-094e2f5711b2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""301ebf75-a037-4e1e-a15c-01d761eb3c04"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""D-Pad"",
                    ""id"": ""cafb397c-fcb4-428c-806b-366b8efdc67e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""da62b96e-13cf-4cf3-ba33-ba6efeec03d2"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""13c8bfa6-db2b-4eab-88c3-8613fac2bc20"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b1c5c71a-70a3-4642-b0e6-8444616e9ad4"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4791da74-9d92-4bc0-b64a-1bfda2887359"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""ae00f559-88b6-465e-88a3-fe78c6495c58"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e2d7eb17-df65-4a4c-a1bc-6a0b8b403e29"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""91bfae9c-81ce-4f9d-b3d7-49781002eaca"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3fb9ebf6-4af9-4f97-96b2-f222fbb5fb13"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""19b00812-df6e-481c-ae61-34939faae2ce"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2b7fc4bd-6cbc-4864-a338-02ef4703de1f"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee6ba62e-0ac8-49ce-9392-318dc231d814"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fdf0e5d0-f26c-4f6d-8e8a-ab5f0b97021b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Craft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e16c677d-697d-443e-9bf2-7f5ec49f44a5"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Craft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard+Mouse"",
            ""bindingGroup"": ""Keyboard+Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // TopDown
        m_TopDown = asset.FindActionMap("TopDown", throwIfNotFound: true);
        m_TopDown_Move = m_TopDown.FindAction("Move", throwIfNotFound: true);
        m_TopDown_Craft = m_TopDown.FindAction("Craft", throwIfNotFound: true);
        // Platformer
        m_Platformer = asset.FindActionMap("Platformer", throwIfNotFound: true);
        m_Platformer_Move = m_Platformer.FindAction("Move", throwIfNotFound: true);
        m_Platformer_Jump = m_Platformer.FindAction("Jump", throwIfNotFound: true);
        m_Platformer_Craft = m_Platformer.FindAction("Craft", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // TopDown
    private readonly InputActionMap m_TopDown;
    private List<ITopDownActions> m_TopDownActionsCallbackInterfaces = new List<ITopDownActions>();
    private readonly InputAction m_TopDown_Move;
    private readonly InputAction m_TopDown_Craft;
    public struct TopDownActions
    {
        private @GameplayInputs m_Wrapper;
        public TopDownActions(@GameplayInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_TopDown_Move;
        public InputAction @Craft => m_Wrapper.m_TopDown_Craft;
        public InputActionMap Get() { return m_Wrapper.m_TopDown; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TopDownActions set) { return set.Get(); }
        public void AddCallbacks(ITopDownActions instance)
        {
            if (instance == null || m_Wrapper.m_TopDownActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_TopDownActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Craft.started += instance.OnCraft;
            @Craft.performed += instance.OnCraft;
            @Craft.canceled += instance.OnCraft;
        }

        private void UnregisterCallbacks(ITopDownActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Craft.started -= instance.OnCraft;
            @Craft.performed -= instance.OnCraft;
            @Craft.canceled -= instance.OnCraft;
        }

        public void RemoveCallbacks(ITopDownActions instance)
        {
            if (m_Wrapper.m_TopDownActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ITopDownActions instance)
        {
            foreach (var item in m_Wrapper.m_TopDownActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_TopDownActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public TopDownActions @TopDown => new TopDownActions(this);

    // Platformer
    private readonly InputActionMap m_Platformer;
    private List<IPlatformerActions> m_PlatformerActionsCallbackInterfaces = new List<IPlatformerActions>();
    private readonly InputAction m_Platformer_Move;
    private readonly InputAction m_Platformer_Jump;
    private readonly InputAction m_Platformer_Craft;
    public struct PlatformerActions
    {
        private @GameplayInputs m_Wrapper;
        public PlatformerActions(@GameplayInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Platformer_Move;
        public InputAction @Jump => m_Wrapper.m_Platformer_Jump;
        public InputAction @Craft => m_Wrapper.m_Platformer_Craft;
        public InputActionMap Get() { return m_Wrapper.m_Platformer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlatformerActions set) { return set.Get(); }
        public void AddCallbacks(IPlatformerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlatformerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlatformerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Craft.started += instance.OnCraft;
            @Craft.performed += instance.OnCraft;
            @Craft.canceled += instance.OnCraft;
        }

        private void UnregisterCallbacks(IPlatformerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Craft.started -= instance.OnCraft;
            @Craft.performed -= instance.OnCraft;
            @Craft.canceled -= instance.OnCraft;
        }

        public void RemoveCallbacks(IPlatformerActions instance)
        {
            if (m_Wrapper.m_PlatformerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlatformerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlatformerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlatformerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlatformerActions @Platformer => new PlatformerActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard+Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface ITopDownActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnCraft(InputAction.CallbackContext context);
    }
    public interface IPlatformerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCraft(InputAction.CallbackContext context);
    }
}
